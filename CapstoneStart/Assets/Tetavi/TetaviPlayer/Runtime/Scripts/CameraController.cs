using UnityEngine;

public static class MatrixExtensions
{
  public static Quaternion ExtractRotation(this Matrix4x4 matrix)
  {
    Vector3 forward;
    forward.x = matrix.m02;
    forward.y = matrix.m12;
    forward.z = matrix.m22;

    Vector3 upwards;
    upwards.x = matrix.m01;
    upwards.y = matrix.m11;
    upwards.z = matrix.m21;

    return Quaternion.LookRotation(forward, upwards);
  }

  public static Vector3 ExtractPosition(this Matrix4x4 matrix)
  {
    Vector3 position;
    position.x = matrix.m03;
    position.y = matrix.m13;
    position.z = matrix.m23;
    return position;
  }

  public static Vector3 ExtractScale(this Matrix4x4 matrix)
  {
    Vector3 scale;
    scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
    scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
    scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
    return scale;
  }
}

public class CameraController : MonoBehaviour
{
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

    // Yigal 09-23-20: Allow orbit mode

  public float mainSpeed = 1.0f; //regular speed
  float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
  float maxShift = 5.0f; //Maximum speed when holdin gshift
  public float camSens = 0.25f; //How sensitive it with mouse
  private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
  private float totalRun = 1.0f;
  bool mouseDown = false;
  public bool orbitMode = true;
  Vector3 pivot = new Vector3(0, 0, 0);

  void UpdateMouse()
  {
    if (!Input.GetMouseButton(0))
    {
      mouseDown = false;
      return;
    }
    if (!mouseDown)  // change state
    {
      lastMouse = Input.mousePosition;
      mouseDown = true;
      return;
    }

    Vector3 updateBy = Input.mousePosition - lastMouse;
    Vector3 updateBy2 = new Vector3(-updateBy.y * camSens, updateBy.x * camSens, 0);
    Matrix4x4 M = transform.worldToLocalMatrix; // view matrix of the camera
    transform.eulerAngles = new Vector3(transform.eulerAngles.x + updateBy2.x, transform.eulerAngles.y + updateBy2.y, 0); ;
    if (orbitMode)
    {                                               // move the position so its relation with the pivot is kept: M*pivot = M'*pivot
      Matrix4x4 Mt = transform.worldToLocalMatrix;  // view matrix of the new camera
      Mt.m03 = Mt.m13 = Mt.m23 = 0;                 // --> Rot Mat only
      Vector4 pivot4 = pivot;
      pivot4.w = 1;                                 // In Unity transform of Matrix 4x4 if full (not 3x3) only if multiplying by vector4 and z=1, specifically
      Vector4 pivotM = M * pivot4;
      Vector4 pivotMt = Mt * pivot4;
      Vector3 newPos = Mt.transpose * (pivotMt - pivotM);  // =>  M*pivot = M'*pivot (M' has the same rotation as M and the new position)
      transform.position = newPos;
    }
    lastMouse = Input.mousePosition;
  }
  void Update()
  {
    if (orbitMode){
      UpdateMouse();
    }
    
    
    //Keyboard commands
    Vector3 p = GetBaseInput();
    // if (!orbitMode)
    {
      if (Input.GetKey(KeyCode.LeftShift))
      {
        totalRun += Time.deltaTime;
        p = p * totalRun * shiftAdd;
        p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
        p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
        p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
      }
      else
      {
        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        p = p * mainSpeed;
      }

      p = p * Time.deltaTime;
      Vector3 newPosition = transform.position;
      if (Input.GetKey(KeyCode.Space))
      { //If player wants to move on X and Z axis only
        transform.Translate(p);
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
      }
      else
      {
        transform.Translate(p);
      }
    }
  }

  private Vector3 GetBaseInput()
  { //returns the basic values, if it's 0 than it's not active.
    Vector3 p_Velocity = new Vector3();
    if (Input.GetKey(KeyCode.W) || Input.GetAxis("Mouse ScrollWheel") > 0f)
    {
      p_Velocity += new Vector3(0, 0, 2);
    }
    if (Input.GetKey(KeyCode.S) || Input.GetAxis("Mouse ScrollWheel") < 0f)
    {
      p_Velocity += new Vector3(0, 0, -2);
    }
    if (Input.GetKey(KeyCode.A))
    {
      p_Velocity += new Vector3(-1, 0, 0);
    }
    if (Input.GetKey(KeyCode.D))
    {
      p_Velocity += new Vector3(1, 0, 0);
    }
    if (Input.GetKey(KeyCode.Q))
    {
      p_Velocity += new Vector3(0, -1, 0);
    }
    if (Input.GetKey(KeyCode.E))
    {
      p_Velocity += new Vector3(0, 1, 0);
    }
    return p_Velocity;
  }
}
