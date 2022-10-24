using System.ComponentModel.DataAnnotations;

namespace Matrices.Matrices;

public class InvertedMatrix : SquareMatrix
{
    public InvertedMatrix(double[,] data) : base(data)
    {
        if (Math.Abs(Determinant) < 0.001)
        {
            throw new ValidationException("Determinant = 0, cannot create invert matrix");
        }
        
        CreateInvertedMatrix();
    }
    
    public InvertedMatrix(Matrix m) : this(m.MatrixData) {}

    private void CreateInvertedMatrix()
    {
        if (_rowCount == 1)
        {
            _matrix[0, 0] = 1 / _matrix[0, 0];
            return;
        }
        
        ProcessFunctionOverMatrix(this, (i, j) =>
        {
            _matrix[i, j] = ((i + j) % 2 == 0 ? 1 : -1) * CreateMinor(i, j).Determinant;
            _matrix[i, j] /= Determinant;
        });

        var tmp = _matrix.Clone() as double[,];
        
        ProcessFunctionOverMatrix(this, (i, j) =>
        {
            _matrix[i, j] = tmp![j, i];
        });
    }
}