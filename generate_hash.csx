#r "C:\Projetos\.NET\TalentBridge\src\TalentBridge.Infrastructure\bin\Debug\net10.0\BCrypt.Net-Next.dll"
Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("Admin@123", workFactor: 11));
