using Microsoft.Data.Sqlite;
using Dapper;
using Winiffer_P1_P4.Models;

namespace Winiffer_P1_P4.Service;

public class AutorService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("SqliteConnection")!;

    private SqliteConnection CreateConnection => new SqliteConnection(_connectionString);

    public async Task InicializeAsync()
    {
        const string consulta = @"
            CREATE TABLE IF NOT EXISTS Autor(
                IdAutor INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombres TEXT NOT NULL,
                Nacionalidad TEXT NOT NULL,
                FechaNacimiento TEXT NOT NULL,
                Sueldo REAL NOT NULL
            );
        ";

        using var connection = CreateConnection;
        await connection.ExecuteAsync(consulta);
    }

    public async Task<int> SaveAsync(AutorRecord autor)
    {
        string consulta = @"
                 INSERT INTO Autor
                (Nombres, Nacionalidad, FechaNacimiento, Sueldo)
                 VALUES
                (@Nombres, @Nacionalidad, @FechaNacimiento, @Sueldo);

                SELECT last_insert_rowid();
        ";

        using var connection = CreateConnection;

        return await connection.ExecuteScalarAsync<int>(consulta, autor);
    }

    public async Task<IEnumerable<AutorRecord>> GetAll()
    {
        using var connection = CreateConnection;

        var consulta = "SELECT * FROM Autor";

        var listado = await connection.QueryAsync<AutorParse>(consulta);

            return listado.Select(a => new AutorRecord(
           (int)a.IdAutor,
           a.Nombres,
           a.Nacionalidad,
           DateTime.Parse(a.FechaNacimiento),
           a.Sueldo
       ));
    }

    public async Task<AutorRecord?> GetByIdAsync(int idAutor)
    {
        using var connection = CreateConnection;

        var consulta = @"
            SELECT * 
            FROM Autor 
            WHERE IdAutor = @IdAutor";

        var autor = await connection.QueryFirstOrDefaultAsync<AutorParse>(consulta, new { IdAutor = idAutor });
        if (autor == null) 
            return null;

        return new AutorRecord(
            (int)autor.IdAutor,
            autor.Nombres,
            autor.Nacionalidad,
            DateTime.Parse(autor.FechaNacimiento),
            autor.Sueldo
        );
    }

    public async Task<int> UpdateAsync(int idAutor, AutorRecord autor)
    {
        string consulta = @"
            UPDATE Autor
            SET
                Nombres = @Nombres,
                Nacionalidad = @Nacionalidad,
                FechaNacimiento = @FechaNacimiento,
                Sueldo = @Sueldo
            WHERE IdAutor = @IdAutor";

        using var connection = CreateConnection;

        return await connection.ExecuteAsync(
            consulta,
            new
            {
                IdAutor = idAutor,
                autor.Nombres,
                autor.Nacionalidad,
                autor.FechaNacimiento,
                autor.Sueldo
            }
        );
    }

    public async Task<int> DeleteAsync(int idAutor)
    {
        string consulta = @"
            DELETE FROM Autor
            WHERE IdAutor = @IdAutor";

        using var connection = CreateConnection;

        return await connection.ExecuteAsync(consulta, new { IdAutor = idAutor }
        );
    }

    public record AutorParse(
        long IdAutor,
        string Nombres,
        string Nacionalidad,
        string FechaNacimiento,
        double Sueldo
    );



}


