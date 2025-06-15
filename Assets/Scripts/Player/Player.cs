using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    Vector2 inputVector;
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float bulletSpeed = 15f;
    private float nextFireTime;
    private Rigidbody2D rb;

    [Header("Ammo")]
    [SerializeField] private int maxMagazine = 30;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int totalAmmo = 120;
    [SerializeField] private float reloadTime = 1.2f;
    private bool isReloading = false;

    [Header("Audio")]
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip reloadSound;
    private AudioSource movementAudioSource;
    private AudioSource shootingAudioSource;
    private AudioSource reloadAudioSource;

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup soundEffectsGroup;

    private float minMovingSpeed = 0.1f;
    private bool isRun = false;

    public int CurrentAmmo => currentAmmo;
    public int TotalAmmo => totalAmmo;

    public static event System.Action<int, int> OnAmmoChanged;

    public event System.Action OnReloadStarted;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();


        movementAudioSource = gameObject.AddComponent<AudioSource>();
        shootingAudioSource = gameObject.AddComponent<AudioSource>();
        reloadAudioSource = gameObject.AddComponent<AudioSource>();

        movementAudioSource.outputAudioMixerGroup = soundEffectsGroup;
        shootingAudioSource.outputAudioMixerGroup = soundEffectsGroup;
        reloadAudioSource.outputAudioMixerGroup = soundEffectsGroup;

    }


    private void Start()
    {
        if (DataManager.Instance.TotalAmmo > 0)
        {
            currentAmmo = DataManager.Instance.CurrentAmmo;
            totalAmmo = DataManager.Instance.TotalAmmo;
        }
        else
        {
            currentAmmo = maxMagazine;
        }


        OnAmmoChanged?.Invoke(currentAmmo, totalAmmo);
    }

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;
        inputVector = GameInput.Instance.GetMovementVector();
        HandleShooting();
        if (GameInput.Instance.IsMedKitUsed())
        {
            GetComponent<PlayerHealth>().UseMedKit();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (currentAmmo <= 0 && !isReloading && totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }

        if (GameInput.Instance.IsReloadPressed() && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void FixedUpdate()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;
        HandleMovement();
        Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);
    }

    private void HandleMovement()
    {

        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        if (isRun && !movementAudioSource.isPlaying)
        {
            movementAudioSource.clip = footstepSound;
            movementAudioSource.Play();
        }
        else if (!isRun && movementAudioSource.isPlaying)
        {
            movementAudioSource.Stop();
        }
    }

    public bool IsRun()
    {
        return isRun;
    }


    private void HandleShooting()
    {
        if (Input.GetMouseButton(0)
         && Time.time >= nextFireTime
         && !isReloading
         && currentAmmo > 0)
        {
            Shoot();
            currentAmmo--;
            OnAmmoChanged?.Invoke(currentAmmo, totalAmmo);
            nextFireTime = Time.time + fireRate;
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed;


        shootingAudioSource.PlayOneShot(shootingSound);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        OnReloadStarted?.Invoke();

        if (reloadSound != null && reloadAudioSource != null)
        {
            reloadAudioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = maxMagazine - currentAmmo;
        int availableAmmo = Mathf.Min(neededAmmo, totalAmmo);

        currentAmmo += availableAmmo;
        totalAmmo -= availableAmmo;

        isReloading = false;
        Debug.Log($"Reloaded! Current ammo: {currentAmmo} | Total left: {totalAmmo}");

        OnAmmoChanged?.Invoke(currentAmmo, totalAmmo);

        DataManager.Instance.CurrentAmmo = currentAmmo;
        DataManager.Instance.TotalAmmo = totalAmmo;
    }


    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
        OnAmmoChanged?.Invoke(currentAmmo, totalAmmo);
    }
    private void OnEnable()
    {
        Transform visualTransform = transform.Find("PlayerVisual");
        if (visualTransform != null)
        {
            visualTransform.gameObject.SetActive(true);
        }

        GetComponent<Collider2D>().enabled = true;
    }
}