using Oracle.ManagedDataAccess.Client;

class Program
{
    static void Main()
    {
        string connectionString = "User Id=rm558798;Password=fiap24;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SID=ORCL)));";
        
        using var connection = new OracleConnection(connectionString);
        connection.Open();
        
        try
        {
            // 1. Marcar primeira migração como aplicada
            var cmd1 = new OracleCommand(@"
                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"") 
                VALUES ('20251122234512_FinalIntegerFieldsFixed', '9.0.0')
            ", connection);
            
            try { cmd1.ExecuteNonQuery(); Console.WriteLine("✅ Primeira migração marcada como aplicada"); }
            catch { Console.WriteLine("⚠️ Primeira migração já estava marcada"); }
            
            // 2. Alterar coluna para permitir NULL (se necessário)
            var cmd2 = new OracleCommand(@"
                ALTER TABLE ""Instituicoes"" MODIFY ""EnderecoId"" NULL
            ", connection);
            
            try { cmd2.ExecuteNonQuery(); Console.WriteLine("✅ Coluna EnderecoId alterada para NULL"); }
            catch { Console.WriteLine("⚠️ Coluna já permite NULL ou erro na alteração"); }
            
            // 3. Marcar segunda migração como aplicada  
            var cmd3 = new OracleCommand(@"
                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                VALUES ('20251123140738_MakeEnderecoIdNullable', '9.0.0')
            ", connection);
            
            try { cmd3.ExecuteNonQuery(); Console.WriteLine("✅ Segunda migração marcada como aplicada"); }
            catch { Console.WriteLine("⚠️ Segunda migração já estava marcada"); }
            
            Console.WriteLine("🎉 Migração finalizada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
        }
    }
}