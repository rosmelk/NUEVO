using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GatoPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    //variables modificables de unity
    public float JumpForce = 10;
    public float Velocity = 10;
    
    private Rigidbody2D _rigidbody2D; //variable 
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool puedeEscalar = false;
    
    //declaramos las constantes
     private const string ANIMATOR_STATE = "Estado";
     private const int ANIMATION_JUMP = 1;
     private const int ANIMATION_RUN = 2;
     private const int ANIMATION_IDLE = 0;
     private const int ANIMATION_SLICE = 3;

     private const int RIGHT = 1;
     private const int LEFT = -1;
     private const int UP= 1;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); //inicializamos y llamamos los componenetes de de RigiDbody
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
        // el objeto su velocidad sera 0 y su animacion.
        _rigidbody2D.velocity = new Vector2(0,_rigidbody2D.velocity.y); 
        ChangeAnimation(ANIMATION_IDLE);
        
        //si presiono la tecla derecha se mueve
        if (Input.GetKey(KeyCode.RightArrow)){
          Desplazarse(RIGHT);
          
        } if(Input.GetKey(KeyCode.LeftArrow)){
            Desplazarse(LEFT);
        } 
        if(Input.GetKey(KeyCode.UpArrow) && puedeEscalar){
          DesplazarceVertical(UP);
        } 
        
        if(Input.GetKey(KeyCode.C))
        {
         Deslizar();
        } 
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            ChangeAnimation(ANIMATION_JUMP);
        }
        
    }

    private void DesplazarceVertical(int position)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Velocity * position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var tag = other.gameObject.tag;
        if (tag == "Obstaculo")
        {
            Debug.Log("Entrar en colision: "+ other.gameObject.name);
        }

        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var tag = other.gameObject.tag;
        if (tag=="Escalable")
        {
            puedeEscalar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var tag = other.gameObject.tag;
        if (tag=="Escalable")
        {
            puedeEscalar = false;
        }  
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("Salir de  colision: "+ other.gameObject.name);
    }

  

    private void Deslizar()
    {
        ChangeAnimation(ANIMATION_SLICE);
    }

    private void Desplazarse(int position)
    {
        _rigidbody2D.velocity = new Vector2(Velocity * position,_rigidbody2D.velocity.y); 
        _spriteRenderer.flipX = position == LEFT;
        ChangeAnimation(ANIMATION_RUN);
    }

    private void ChangeAnimation(int animation)
    {
        _animator.SetInteger(ANIMATOR_STATE, animation);
    }
    
    

}
