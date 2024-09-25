// Author: Mathias Soeholm
// Date: 05/10/2016
// No license, do whatever you want with this script

using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class TubeRenderer : MonoBehaviour
{
    [SerializeField] Vector3[] _positions;
    [SerializeField] int _sides = 8;
    [SerializeField] float _radiusOne = 0.01f;  // Grosor inicial
    [SerializeField] float _radiusTwo = 1f;  // Grosor final
    [SerializeField] float _middleRadius = 0.015f;  // Grosor intermedio
    [SerializeField] bool _useWorldSpace = true;
    [SerializeField] bool _useTwoRadii = false;

    private Vector3[] _vertices;
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;


    public Material material
    {
        get { return _meshRenderer.material; }
        set { _meshRenderer.material = value; }
    }

    public void DestroyCable()
    {
        _mesh = null;
        _meshFilter = null;
        _meshRenderer = null;
    }
    
    private void OnEnable()
    {
        if (_mesh != null)
            _meshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        if (_mesh != null)
            _meshRenderer.enabled = false;
    }
    
    private void OnValidate()
    {
        _sides = Mathf.Max(3, _sides);
    }

    public void SetPositions(Vector3[] positions, Transform objectA, Transform objectB, GameObject cable)
    {
        _positions = positions;
        GenerateMesh(objectA, objectB, cable);
    }

    private void GenerateMesh(Transform objectA, Transform objectB, GameObject cable)
    {
        if (_mesh == null || _positions == null || _positions.Length <= 1)
        {
            //GameObject line = new GameObject();
            
            //line.name = "Cable " + objectA.GetComponent<InOutController>().component.componentName + " - " + objectA.name
                        //+ " / " + objectB.GetComponent<InOutController>().component.componentName + " - " + objectB.name;

            _meshFilter = cable.AddComponent<MeshFilter>();
            _meshRenderer = cable.AddComponent<MeshRenderer>();
            
            _mesh = new Mesh();
        
        }

        var verticesLength = _sides * _positions.Length;
        if (_vertices == null || _vertices.Length != verticesLength)
        {
            _vertices = new Vector3[verticesLength];

            var indices = GenerateIndices();
            var uvs = GenerateUVs();

            if (verticesLength > _mesh.vertexCount)
            {
                _mesh.vertices = _vertices;
                _mesh.triangles = indices;
                _mesh.uv = uvs;
            }
            else
            {
                _mesh.triangles = indices;
                _mesh.vertices = _vertices;
                _mesh.uv = uvs;
            }
        }

        var currentVertIndex = 0;
        Vector3 previousUp = Vector3.up;

        for (int i = 0; i < _positions.Length; i++)
        {
            var circle = CalculateCircle(i, ref previousUp);
            foreach (var vertex in circle)
            {
                _vertices[currentVertIndex++] = _useWorldSpace ? transform.InverseTransformPoint(vertex) : vertex;
            }
        }

        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _meshFilter.mesh = _mesh;
    }

    /*private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[_positions.Length * _sides];

        for (int segment = 0; segment < _positions.Length; segment++)
        {
            for (int side = 0; side < _sides; side++)
            {
                var vertIndex = (segment * _sides + side);
                var u = side / (_sides - 1f);
                var v = segment / (_positions.Length - 1f);

                uvs[vertIndex] = new Vector2(u, v);
            }
        }

        return uvs;
    }*/

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[_positions.Length * _sides];

        for (int segment = 0; segment < _positions.Length; segment++)
        {
            float v = (float)segment / (_positions.Length - 1);

            for (int side = 0; side < _sides; side++)
            {
                float u = (float)side / (_sides - 1);
                int vertIndex = segment * _sides + side;
                uvs[vertIndex] = new Vector2(u, v);
            }
        }

        return uvs;
    }



    private int[] GenerateIndices()
    {
        var indices = new int[_positions.Length * _sides * 2 * 3];

        var currentIndicesIndex = 0;
        for (int segment = 1; segment < _positions.Length; segment++)
        {
            for (int side = 0; side < _sides; side++)
            {
                var vertIndex = (segment * _sides + side);
                var prevVertIndex = vertIndex - _sides;

                indices[currentIndicesIndex++] = prevVertIndex;
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                indices[currentIndicesIndex++] = vertIndex;

                indices[currentIndicesIndex++] = (side == _sides - 1) ? (prevVertIndex - (_sides - 1)) : (prevVertIndex + 1);
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                indices[currentIndicesIndex++] = prevVertIndex;
            }
        }

        return indices;
    }

    private Vector3[] CalculateCircle(int index, ref Vector3 previousUp)
    {
        var dirCount = 0;
        var forward = Vector3.zero;

        if (index > 0)
        {
            forward += (_positions[index] - _positions[index - 1]).normalized;
            dirCount++;
        }

        if (index < _positions.Length - 1)
        {
            forward += (_positions[index + 1] - _positions[index]).normalized;
            dirCount++;
        }

        forward = (forward / dirCount).normalized;
        var side = Vector3.Cross(previousUp, forward).normalized;
        var up = Vector3.Cross(forward, side).normalized;
        previousUp = up;

        var circle = new Vector3[_sides];
        var angle = 0f;
        var angleStep = (2 * Mathf.PI) / _sides;

        float radius;
        if (index == 0 || index == _positions.Length - 1)  // Check if it's the first or last point
        {
            radius = _middleRadius;  // Use _middleRadius for first and last points
        }
        else if (index == 1 || index == _positions.Length - 2)  // Check if it's the second or second-to-last point
        {
            radius = _middleRadius;  // Use _middleRadius for second and second-to-last points
        }
        else
        {
            radius = _radiusOne;
        }

        for (int i = 0; i < _sides; i++)
        {
            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);

            circle[i] = _positions[index] + side * x * radius + up * y * radius;

            angle += angleStep;
        }

        return circle;
    }


}
