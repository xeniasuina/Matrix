using System.ComponentModel.DataAnnotations;

namespace Matrices.Matrices;

public class SquareMatrix : Matrix
{
    private double _determinant;
    private List<int> _linearlyDependentRows = new();
    private List<int> _linearlyDependentColumns = new();
    
    public double Determinant => _determinant;

    public SquareMatrix(int rowCount) : base(rowCount) { }

    public SquareMatrix(double[,] data) : base(data)
    {
        if (_rowCount != _columnCount)
            throw new ValidationException("Dimensions is not equal");

        _determinant = CalculateDeterminant();
    }

    public SquareMatrix(Matrix matrix) : this(matrix.MatrixData) { }

    public void RemoveLinearlyDependentRowsAndColumns()
    {
        if (!(Math.Abs(_determinant) < 0.001))
        {
            return;
        }

        var res = new Matrix(_matrix);

        foreach (var row in _linearlyDependentRows)
        {
            res.CreateMatrixWithoutRow(row);
        }

        foreach (var column in _linearlyDependentColumns)
        {
            res.CreateMatrixWithoutColumn(column);
        }

        _matrix = new double[res.RowCount, res.ColumnCount];
        ProcessFunctionOverMatrix(this, (i, j) => _matrix[i, j] = res[i, j]);
    }

    private double CalculateDeterminant()
    {
        var res = new Matrix(this);
        var det = 1.0;
        const double eps = 1.00E-4;
        
        for (var i = 0; i < _rowCount; i += 1)
        {
            var k = i;
            for (var j = i + 1; j < _columnCount; j += 1)
            {
                if (Math.Abs(res[j, i]) > Math.Abs(res[k, i]))
                {
                    k = j;
                }
            }
            
            if (Math.Abs(res[k, i]) < eps)
            {
                det = 0;
                _linearlyDependentRows.Add(i);
                _linearlyDependentColumns.Add(k);
                break;
            }
            
            SwapRows(ref res, i, k);
            if (i != k)
            {
                det *= -1;
            }
            det *= res[i, i];

            for (var j = i + 1; j < _columnCount; j += 1)
            {
                res[i, j] /= res[i, i];
            }
            
            for (var j = 0; j < _columnCount; j += 1)
            {
                if (j != i && Math.Abs(this[j, i]) > eps)
                {
                    for (var s = i + 1; s < _columnCount; s += 1)
                    {
                        res[j, s] -= res[i, s] * res[j, i];
                    }
                }
            }
        }
        
        return det;
    }

    protected SquareMatrix CreateMinor(int row, int column) =>
        new(CreateMatrixWithoutRow(row).CreateMatrixWithoutColumn(column));
}
