using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    void Start ()
    {
        GameObject heroe = GameObject.CreatePrimitive(PrimitiveType.Cube);
        heroe.AddComponent<GenerarHeroe>();

        for (int i = 0; i < Random.Range(10,21); i++)
        {
            int valorGeneracion = Random.Range(0, 2);
            if (valorGeneracion == 0)
            {
                GameObject aldeano = GameObject.CreatePrimitive(PrimitiveType.Cube);
                aldeano.name = "Aldeano";
                aldeano.AddComponent<GenerarAldeano>();
            }
            else
            {
                GameObject zombie = GameObject.CreatePrimitive(PrimitiveType.Cube);
                zombie.name = "Zombie";
                zombie.AddComponent<GenerarZombie>();
            }
        }
	}
}

/// <summary>
/// este componente se encarga de asignarle camara,rigidbody (Se desactiva la gravedad y se activan todos los constrains), velocidad al azar e inicia una corrutina para los controladores del heroe al objeto que lo tenga
/// </summary>
public class GenerarHeroe : MonoBehaviour
{
    private IEnumerator movCo;
    float vel;

    public void Start()
    {
        gameObject.AddComponent<Camera>();
        Rigidbody playerRigid = gameObject.AddComponent<Rigidbody>();
        playerRigid.useGravity = false;
        playerRigid.constraints = RigidbodyConstraints.FreezeAll;
        movCo = Acciones();
        tag = "Player";
        vel = Random.Range(0.2f,0.5f);
        StartCoroutine(movCo);
    }

    /// <summary>
    /// Esta corrutina se encarga de asignar las clases movimiento y rotacion, aparte de ejecutarlas cada frame
    /// </summary>
    /// <returns></returns>
    public IEnumerator Acciones()
    {

        Movimiento movimiento = new Movimiento();
        Rotacion rotacion = new Rotacion();

        while (true)
        {
            movimiento.Mover(this.gameObject, vel);
            rotacion.Girar(this.gameObject, vel);

            yield return new WaitForEndOfFrame();
        }
    }
}

/// <summary>
/// Permite que el objeto que lo tenga se mueva adelante y atras con W y S
/// </summary>
public class Movimiento
{
    public void Mover(GameObject x, float vel)
    {
        if (Input.GetKey(KeyCode.W))
        {
            x.transform.Translate(0, 0, vel / 4);
        }
        if (Input.GetKey(KeyCode.S))
        {
           x.transform.Translate(0, 0, -vel / 4);
        }
    }
}

/// <summary>
/// Permite que el objeto que lo tenga que gire a su mirada a los lados con A y D
/// </summary>
public class Rotacion
{
    public void Girar(GameObject z,float vel)
    {
        if (Input.GetKey(KeyCode.A))
        {
            z.transform.Rotate(0, -vel * 4, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            z.transform.Rotate(0, vel * 4, 0);
        }
    }
}

/// <summary>
/// Esta clase al iniciar agrega un gusto y color al objeto, aparte de guarlar la informacion en un struct para cada objeto y asignar los componentes zombieStatus y ZombieCollision
/// </summary>
public class GenerarZombie:MonoBehaviour
{
    string gusto;

    enum Gusto
    {
        Piernas,Dedos,Cerebro,Ojos,Lengua,lenght
    }

    public void Start()
    {
        GameObject zombie = this.gameObject;
        ZombieData zombieData = new ZombieData();
        zombie.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));

        Renderer zRender = zombie.GetComponent<Renderer>();
        int numeroColor = Random.Range(0, 3);
        if(numeroColor == 0)
            zRender.material.color = Color.cyan;
        else if(numeroColor == 1)
            zRender.material.color = Color.green;
        else
            zRender.material.color = Color.magenta;

        Gusto enumGusto;
        enumGusto = (Gusto)Random.Range(0, (int)Gusto.lenght);
        gusto = enumGusto.ToString();
        zombieData.Gusto = gusto;

        zombie.AddComponent<ZombieCollision>().TakeData(zombieData);
        zombie.AddComponent<ZombieStatus>();
    }
}

/// <summary>
/// Esta clase maneja el estado del objeto entre Idle y Mov recalculando estado cada 5 Segundos
/// </summary>
public class ZombieStatus : MonoBehaviour
{
    float timer;

    enum Estado
    {
        Idle,Movi
    }

    private void Start()
    {
        IEnumerator mov = CambiarEstado();
        StartCoroutine(mov);
    }

    IEnumerator CambiarEstado()
    {
        while (true)
        {
        Estado estado;
        estado = (Estado)Random.Range(0, 2);

        switch (estado)
        {
            case Estado.Idle:
                    yield return new WaitForSeconds(5);
                break;
            case Estado.Movi:
                    int girar = Random.Range(0, 360);
                    transform.Rotate(0, girar, 0);
                    while (timer < 5)
                    {
                        transform.Translate(0,0,0.025f);
                        timer += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                    break;
        }
            if(timer > 5)
            {
                timer = 0;
            }
        }
    }
}

/// <summary>
/// Esta clase se encarga de detectar la collision con el jugador y mandar el mensaje que se requiere
/// </summary>
public class ZombieCollision : MonoBehaviour
{
    ZombieData zombieDat = new ZombieData();

    public void TakeData(ZombieData zombieData)
    {
        zombieDat = zombieData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            print("Waaaarrrr quier comer " + zombieDat.Gusto);
    }
}

/// <summary>
/// Esta clase se encarga de darle un nombre y edad al objeto almacenando esta informacion en un struct, luego le agrega un componente AldeanoCollision y le envia el struct creado
/// </summary>
public class GenerarAldeano:MonoBehaviour
{

    VillagerData villagerData = new VillagerData();

    int edad;
    string nombre;

    enum Nombres
    {
        Alejandro, Daniel, Julio, Danilo, Kevin, Brayan, Juan, Sebastian, Luis, Alex, Jorge, Anderson, Cristian, Camilo, Carlos, Felipe, Andres, Gustavo, Cesar, andres, lenght
    }

    public void Start()
    {
        GameObject aldeano = this.gameObject;
        Nombres nombres;

        aldeano.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));

        edad = Random.Range(15, 101);
        int numeroNombre = Random.Range(0,20);
        nombres = (Nombres)numeroNombre;
        nombre = nombres.ToString();

        villagerData.Age = edad;
        villagerData.name = nombre;

        aldeano.AddComponent<AldeanoCollision>().TakeData(villagerData);
    }
}

/// <summary>
/// Esta clase se encarga de guardar los datos del objeto y de enviarlos en un mensaje cuando detecta al heroe
/// </summary>
public class AldeanoCollision : MonoBehaviour
{
    VillagerData villagerDat = new VillagerData();

    /// <summary>
    /// Almacena un struct recivido para enviarlo luego en collision
    /// </summary>
    /// <param name="villagerData">
    /// Informacion del objeto en un struct
    /// </param>
    public void TakeData(VillagerData villagerData)
    {
        villagerDat = villagerData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            print("Hola soy " + villagerDat.name + " y tengo " + villagerDat.Age + " años");
    }
}
