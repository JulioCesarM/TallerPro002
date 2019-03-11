using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    void Start () {

        GameObject Heroe = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Heroe.AddComponent<GenerarHeroe>();

        for (int i = 0; i < Random.Range(10,21); i++)
        {
            int x = Random.Range(0, 2);
            if (x == 0)
            {
                GameObject Aldeano = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Aldeano.name = "Aldeano";
                Aldeano.AddComponent<GenerarAldeano>();
            }
            else
            {
                GameObject Zombie = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Zombie.name = "Zombie";
                Zombie.AddComponent<GenerarZombie>();
            }
        }

	}
}


public class GenerarHeroe : MonoBehaviour
{

    private IEnumerator movCo;

    float vel;
    public void Start()
    {
        gameObject.AddComponent<Camera>();
        Rigidbody PlayerRigid = gameObject.AddComponent<Rigidbody>();
        PlayerRigid.useGravity = false;
        PlayerRigid.constraints = RigidbodyConstraints.FreezeAll;
        movCo = Acciones();
        tag = "Player";
        vel = Random.Range(0.2f,0.5f);
        StartCoroutine(movCo);
    }

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

public class GenerarZombie:MonoBehaviour
{

    string gusto;

    enum Gusto
    {
        Piernas,Dedos,Cerebro,Ojos,Lengua,lenght
    }

    public void Start()
    {
        GameObject Zombie = this.gameObject;
        ZombieData zombieData = new ZombieData();
        Gusto enumGusto;

        Zombie.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));

        Renderer ZRender = Zombie.GetComponent<Renderer>();
 
        int x = Random.Range(0, 3);

        if(x == 0)
        
            ZRender.material.color = Color.cyan;
        else if(x == 1)
            ZRender.material.color = Color.green;
        else
            ZRender.material.color = Color.magenta;
        


        enumGusto = (Gusto)Random.Range(0, (int)Gusto.lenght);

        gusto = enumGusto.ToString();
        zombieData.Gusto = gusto;
        Zombie.AddComponent<ZombieCollision>().TakeData(zombieData);
        Zombie.AddComponent<ZombieStatus>();

    }
}


public class ZombieStatus : MonoBehaviour
{
    float timer;
    bool girado;



    enum Estado
    {
        Idle,Movi
    }

    private void Start()
    {
        IEnumerator Mov = CambiarEstado();
        StartCoroutine(Mov);
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
                
            girado = true;
        }
      
    }

}


public class ZombieCollision : MonoBehaviour
{

    ZombieData ZombieDat = new ZombieData();

    public void TakeData(ZombieData zombieData)
    {
        ZombieDat = zombieData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            print("Waaaarrrr quier comer " + ZombieDat.Gusto);
    }

}



public class GenerarAldeano:MonoBehaviour
{

    VillagerData VillagerData = new VillagerData();

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
        int Nombre = Random.Range(0,20);
        nombres = (Nombres)Nombre;
        nombre = nombres.ToString();

        VillagerData.Age = edad;
        VillagerData.name = nombre;

        aldeano.AddComponent<AldeanoCollision>().TakeData(VillagerData);
    }
}

public class AldeanoCollision : MonoBehaviour
{
    VillagerData villagerDat = new VillagerData();

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
