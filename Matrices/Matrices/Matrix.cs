using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Matrices.Matrices;

public class Matrix
{
    private protected int _columnCount;

    private protected int _rowCount;

    private protected double[,] _matrix;

    public int ColumnCount => _columnCount;

    public int RowCount => _rowCount;

    public double[,] MatrixData => _matrix;

    public Matrix(int columnCount, int rowCount)
    {
        _columnCount = columnCount;
        _rowCount = rowCount;

        _matrix = new double[_rowCount, _columnCount];
        ProcessFunctionOverMatrix(this, (i, j) => this[i, j] = (i + j) % 2 == 0 ? 1 : 0);
    }

    public Matrix(int rowCount)
    {
        _columnCount = rowCount;
        _rowCount = rowCount;

        _matrix = new double[_rowCount, _columnCount];
        
        ProcessFunctionOverMatrix(this, (i, j) =>
        {
            if (i == j)
                this[i, j] = 1.0;
        });
    }

    public Matrix(double[,] data)
    {
        _rowCount = data.GetLength(0);
        _columnCount = data.GetLength(1);

        _matrix = new double[_rowCount, _columnCount];
        ProcessFunctionOverMatrix(this, (i, j) => _matrix[i, j] = data[i, j]);
    }

    public Matrix(Matrix other)
    {
        _columnCount = other.ColumnCount;
        _rowCount = other.RowCount;

        _matrix = new double[_rowCount, _columnCount];
        _matrix = (other.MatrixData.Clone() as double[,])!;
    }

    public double this[int i, int j]
    {
        get
        {
            if (i * j < 0 || i >= RowCount || j >= ColumnCount)
                throw new ValidationException();
            return _matrix[i, j];
        }
        set
        {
            if (i * j < 0 || i >= RowCount || j >= ColumnCount)
                throw new ValidationException();
            _matrix[i, j] = value;
        }
    }

    public static Matrix operator *(Matrix m, double lambda)
    {
        var res = new Matrix(m);
        ProcessFunctionOverMatrix(res, (i, j) => res[i, j] = m[i, j] * lambda);

        return res;
    }

    public static Matrix operator +(Matrix m1, Matrix m2)
    {
        if (!Validate(m1, m2))
        {
            Debug.WriteLine("Invalid matrices");
            return new Matrix(1);
        }
        
        var res = new Matrix(m1);
        ProcessFunctionOverMatrix(res, (i, j) => res[i, j] = m1[i, j] + m2[i, j]);

        return res;
    }

    public static Matrix operator +(Matrix m, double lambda)
    {
        var res = new Matrix(m);
        ProcessFunctionOverMatrix(res, (i, j) => res[i, j] = m[i, j] + lambda);

        return res;
    }

    public static Matrix operator -(Matrix m1, Matrix m2)
    {
        if (!Validate(m1,m2))
        {
            Debug.WriteLine("Invalid matrices");
            return new Matrix(1);
        }
        var res = new Matrix(m1 + (m2 * -1.0));
        return res;
    }

    public static Matrix operator -(Matrix m, double lambda)
    {
        var res = new Matrix(m + -lambda);
        return res;
    }

    public static Matrix operator *(Matrix m1, Matrix m2)
    {
        if (!ValidateForMultiplication(m1, m2))
        {
            Debug.WriteLine("Invalid matrices");
            return new Matrix(1);
        }
        var res = new Matrix(m1.ColumnCount, m2.RowCount);
        
        ProcessFunctionOverMatrix(res, (i, j) =>
        {
            for (var k = 0; k < m1.ColumnCount; k += 1)
            {
                res[i, j] += m1[i, k] * m2[k, j];
            }
        });

        return res;
    }

    public static Matrix operator /(Matrix m, double lambda)
    {
        if (Math.Abs(lambda) < 0.001)
        {
            Debug.WriteLine("Division by zero");
            return new Matrix(1);
        }

        return new Matrix(m * (1.0 / lambda));
    }

    /// <summary>
    /// Транспонирование матрицы.
    /// </summary>
    public static Matrix operator ~(Matrix m)
    {
        var res = new Matrix(m);
        ProcessFunctionOverMatrix(res, (i, j) => res[i, j] = m[j, i]);

        return res;
    }

    public override int GetHashCode() => _matrix.GetHashCode();

    public static bool operator ==(Matrix m1, Matrix m2) => m1.GetHashCode() == m2.GetHashCode();

    public static bool operator !=(Matrix m1, Matrix m2) => !(m1 == m2);

    public override bool Equals(object? obj) => obj is Matrix other && this == other;

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < _rowCount; i += 1)
        {
            for (var j = 0; j < _columnCount; j += 1)
            {
                sb.Append($"{_matrix[i, j]:G5}" + (j == _columnCount - 1 ? "" : " "));
            }

            sb.Append('\n');
        }

        return sb.ToString();
    }

    private protected static void ProcessFunctionOverMatrix(Matrix m, Action<int, int> func)
    {
        for (var i = 0; i < m.RowCount; i += 1)
        {
            for (var j = 0; j < m.ColumnCount; j += 1)
            {
                func(i, j);
            }
        }
    }

    private static bool Validate(Matrix m, Matrix other) => m.RowCount == other.RowCount && m.ColumnCount == other.ColumnCount;

    private static bool ValidateForMultiplication(Matrix m1, Matrix m2) => m1.ColumnCount == m2.RowCount;
    
    private protected static Matrix SwapRows(ref Matrix res, int row1, int row2)
    {
        for (var i = 0; i < res.ColumnCount; i += 1)
        {
            (res[row1, i], res[row2, i]) = (res[row2, i], res[row1, i]);
        }
        return res;
    }
    
    protected internal Matrix CreateMatrixWithoutColumn(int column)
    {
        if (column < 0 || column >= _columnCount)
        {
            throw new ArgumentException("invalid column index");
        }
        
        var result = new Matrix(_rowCount, _columnCount - 1);
        ProcessFunctionOverMatrix(result,(i, j) => result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
        return result;
    }

    protected Matrix CreateMatrixWithoutRow(int row)
    {
        if (row < 0 || row >= _rowCount)
        {
            throw new ArgumentException("invalid row index");
        }
        
        var result = new Matrix(_rowCount - 1, _columnCount);
        ProcessFunctionOverMatrix(result, (i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
        return result;
    }

}