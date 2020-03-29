using UnityEngine;

[ExecuteInEditMode()]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class SoftSprite: MonoBehaviour
{
	public Texture Sprite;
    public Vector2 Scale = Vector2.one;

    private MeshRenderer meshRenderer;
	private MeshFilter meshFilter;

	private const float PixelPerMeter = 100f;
	private readonly Color32 Color = new Color32(255, 255, 255, 255);

	void Awake()
	{      
        meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
        CreateMesh();
        ForceUpdate();
    }

	private void CreateMesh()
	{
        //var material = new Material(Shader.Find("Sprites/Default"));

        UpdateMesh();
		//UpdateTexture(material);

		//meshRenderer.sharedMaterial = material;
	}

    private void Update()
    {
        UpdateMesh();
    }

	private void UpdateMesh()
	{
		if (Sprite == null) return;

		var mesh = new Mesh();
        Vector2 ratio = Scale;
        mesh.vertices = new[]
        {
            new Vector3(0f * ratio.x, 0f * ratio.y, 0),
            new Vector3( 0f * ratio.x, -1f * ratio.y, 0),
            new Vector3(0f * ratio.x,  -2f * ratio.y, 0),
            new Vector3( 0f * ratio.x,  -3f * ratio.y, 0),
			new Vector3( 0f * ratio.x, -4f * ratio.y, 0),
			new Vector3(0f * ratio.x,  -5f * ratio.y, 0),
			new Vector3( 0f * ratio.x,  -6f * ratio.y, 0),

            new Vector3(1f * ratio.x, 0f * ratio.y, 0),
            new Vector3( 1f * ratio.x, -1f * ratio.y, 0),
            new Vector3(1f * ratio.x, -2f * ratio.y, 0),
            new Vector3( 1f * ratio.x,  -3f * ratio.y, 0),
            new Vector3( 1f * ratio.x, -4f * ratio.y, 0),
            new Vector3(1f * ratio.x,  -5f * ratio.y, 0),
            new Vector3( 1f * ratio.x,  -6f * ratio.y, 0),

            new Vector3(-1f * ratio.x, 0f * ratio.y, 0),
            new Vector3( -1f * ratio.x, -1f * ratio.y, 0),
            new Vector3(-1f * ratio.x,  -2f * ratio.y, 0),
            new Vector3( -1f * ratio.x,  -3f * ratio.y, 0),
            new Vector3( -1f * ratio.x, -4f * ratio.y, 0),
            new Vector3(-1f * ratio.x,  -5f * ratio.y, 0),
            new Vector3( -1f * ratio.x, -6f * ratio.y, 0),
        };
		
		mesh.triangles = new[]
		{
			15,14,0,
            15,0,1,
            1,0,7,
            1,7,8,
            16,15,1,
            16,1,2,
            2,1,8,
            2,8,9,
            17,16,2,
            17,2,3,
            3,2,9,
            3,9,10,
            18,17,3,
            18,3,4,
            4,3,10,
            4,10,11,
            19,18,4,
            19,4,5,
            5,4,11,
            5,11,12,
            20,19,5,
            20,5,6,
            6,5,12,
            6,12,13

		};
   ratio = new Vector2(0.5f, 1/6f);

        mesh.uv = new[]
        {                                                                             
               new Vector2(1* ratio.x, 6* ratio.y),
               new Vector2(1* ratio.x, 5* ratio.y),
                new Vector2(1* ratio.x, 4* ratio.y),
                 new Vector2(1* ratio.x, 3* ratio.y),
                  new Vector2(1* ratio.x, 2* ratio.y),
                  new Vector2(1* ratio.x, 1* ratio.y),
                   new Vector2(1* ratio.x, 0* ratio.y),
          
              
               new Vector2(2* ratio.x, 6* ratio.y),
                new Vector2(2* ratio.x, 5* ratio.y),

                new Vector2(2* ratio.x, 4* ratio.y),

                 new Vector2(2* ratio.x, 3* ratio.y),
                     new Vector2(2* ratio.x, 2* ratio.y),
                       new Vector2(2* ratio.x, 1* ratio.y),
                        new Vector2(2* ratio.x, 0* ratio.y),

            new Vector2(0* ratio.x, 6* ratio.y),
            new Vector2(0* ratio.x, 5* ratio.y),
             new Vector2(0* ratio.x, 4* ratio.y),
              new Vector2(0* ratio.x, 3* ratio.y),
            new Vector2(0* ratio.x, 2* ratio.y),
              new Vector2(0* ratio.x, 1* ratio.y),

                new Vector2(0* ratio.x, 0* ratio.y),
      


            //  new Vector2(1* ratio.x, 0* ratio.y),
            //   new Vector2(1* ratio.x, 1* ratio.y),
            //   new Vector2(1* ratio.x, 2* ratio.y),
            //   new Vector2(1* ratio.x, 3* ratio.y),
            //   new Vector2(1* ratio.x, 4* ratio.y),
            //   new Vector2(1* ratio.x, 5* ratio.y),
            //   new Vector2(1* ratio.x, 6* ratio.y),





            //  new Vector2(2* ratio.x, 0* ratio.y),
            //   new Vector2(2* ratio.x, 1* ratio.y),
            //   new Vector2(2* ratio.x, 2* ratio.y),
            //   new Vector2(2* ratio.x, 3* ratio.y),
            //   new Vector2(2* ratio.x, 4* ratio.y),
            //   new Vector2(2* ratio.x, 5* ratio.y),
            //   new Vector2(2* ratio.x, 6* ratio.y),


            //    new Vector2(0* ratio.x, 0* ratio.y),
            //new Vector2(0* ratio.x, 1* ratio.y),
            //new Vector2(0* ratio.x, 2* ratio.y),
            //new Vector2(0* ratio.x, 3* ratio.y),
            //new Vector2(0* ratio.x, 4* ratio.y),
            //new Vector2(0* ratio.x, 5* ratio.y),
            //new Vector2(0* ratio.x, 6* ratio.y),
        };
		
		mesh.colors32 = new[]
		{Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
			Color,
			Color,
			Color
		};
		
		meshFilter.sharedMesh = mesh;
	}

	private void UpdateTexture(Material material)
	{
		material.mainTexture = Sprite;
        material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        material.SetInt("_ZWrite", 0);
        material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    }

	public void ForceUpdate()
	{
		if (gameObject.activeInHierarchy)
		{
			UpdateMesh();
			UpdateTexture(meshRenderer.sharedMaterial);
		}
	}
}