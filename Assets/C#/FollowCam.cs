using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Ссылка на интересующий объект // а
    [Header("Set Dynamically")]
    public float camZ; // Желаемая координата Z камеры
    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        // Однострочная версия if не требует фигурных скобок
      //  if (POI == null) return; // выйти, если нет интересующего объекта // b
        // Получить позицию интересующего объекта
      //  Vector3 destination = POI.transform.position;

        Vector3 destination;
        // Если нет интересующего объекта, вернуть Р:[ 0, 0, 0 ]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            // Получить позицию интересующего объекта
            destination = POI.transform.position;
            // Если интересующий объект - снаряд, убедиться, что он остановился
            if (POI.tag == "Projectile")
            {
                // Если он стоит на месте(то есть не двигается)
            if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // Вернуть исходные настройки поля зрения камеры
                    POI = null;
                    //в следующем кадре
                    return;
                }
            }
        }

        // Принудительно установить значение destination.z равным camZ, чтобы
        // отодвинуть камеру подальше
        destination.z = camZ;
        if (destination.y < 0) destination.y = 0;
        if (destination.x < 0) destination.x = 0;
        // Поместить камеру в позицию destination
        this.transform.position = destination;
        // Изменить размер orthographicSize камеры., чтобы земля
        // оставалась в поле зрения
        Camera.main.orthographicSize = destination.y + 10;
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
