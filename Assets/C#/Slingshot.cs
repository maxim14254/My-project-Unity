
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    // поля, устанавливаемые в инспекторе Unity
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    [Header("Set Dynamically")] 
    public GameObject launchPoint;
    public Vector3 launchPos; 
    public GameObject projectile; 
    public bool aimingMode;
    private Rigidbody projectileRigidbody;
    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint"); // a
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
       
        launchPos = launchPointTrans.position;
    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Если рогатка не в режиме прицеливания, не выполнять этот код
        if (!aimingMode) return;
        // Получить текущие координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition; 
        
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        
        // Найти разность координат между launchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Ограничить mouseDelta радиусом коллайдера объекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize(); // устанавливает длинну вектора равной 1
            mouseDelta *= maxMagnitude;
        }
        // Передвинуть снаряд в новую позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        { 
          // Кнопка мыши отпущена
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
        }
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
        //print("Slingshot:OnMouseEnter()");
    }
    void OnMouseExit()
    {
        launchPoint.SetActive(false);
        //print("Slingshot:OnMouseExit()");
    }
    void OnMouseDown()
    { 
      // Игрок нажал кнопку мыши, когда указатель находился над рогаткой
        aimingMode = true;
        // Создать снаряд
        projectile = Instantiate<GameObject>(prefabProjectile);
        // Поместить в точку launchPoint
        projectile.transform.position = launchPos;
        // Сделать его кинематическим
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
}
