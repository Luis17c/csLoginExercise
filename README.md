<h1> csLoginSys </h1>

Sistema básico de Login feito em C# usando .NET
Implementa token JWT e validação dos requests com FluentValidation.

<h2> Configuração Inicial </h2>

Declarando key JWT (É necessário que a key tenha mais de 256 bytes): ````dotnet user-secrets "Token:Key" "your-secret"````

Buildando as imagems: ````docker-compose build````

Rodando migrations: ````dotnet ef database update````

Instanciando as imagens: ````docker-compose up````

Em ambiente de desenvolvimento, as imagens são armazenadas localmente na pasta Tmp, em produção é instanciado o armazenamento pela AWS, porém a implementação dos métodos é necessária.

Postgres exposto em 5432 e o app em 5161.

Com servidor rodando, swagger disponível em http://your-ip:5161/swagger/index.html
