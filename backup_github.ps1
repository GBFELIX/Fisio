# 1. Configurações
$RepoPath = "C:\Users\T-GAMER\source\repos\PlanTributario"
$BackupFile = "$RepoPath\db_backup.sql"
$DatabaseName = "PlanTributarioContext-..." # Verifique o nome exato no seu SQL Object Explorer

# 2. Entra na pasta do projeto
cd $RepoPath

# 3. Exporta os dados da tabela para um arquivo SQL
# Usamos o sqlcmd para gerar os INSERTS atuais
sqlcmd -S "(localdb)\mssqllocaldb" -d $DatabaseName -Q "SET NOCOUNT ON; SELECT 'INSERT INTO Lancamentoes (DataLancamento, Descricao, ValorBruto, ProLabore, ImpostoDAS, INSS, TipoAtendimento) VALUES (''' + CONVERT(VARCHAR, DataLancamento, 120) + ''', ''' + Descricao + ''', ' + CAST(ValorBruto AS VARCHAR) + ', ' + CAST(ProLabore AS VARCHAR) + ', ' + CAST(ImpostoDAS AS VARCHAR) + ', ' + CAST(INSS AS VARCHAR) + ', ''' + TipoAtendimento + ''');' FROM Lancamentoes" -o $BackupFile

# 4. Envia para o GitHub
git add db_backup.sql
git commit -m "Backup automático: $(Get-Date -Format 'dd/MM/yyyy HH:mm')"
git push origin main