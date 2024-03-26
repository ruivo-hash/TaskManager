# API TaskManager

### Endpoints
- Tarefas: <br>
GET /tasks: Retorna todas as tarefas criadas pelo usuário autenticado. <br>
GET /tasks/{id}: Retorna a tarefa do id informado se ela pertencer ao usuário. <br>
POST /tasks: Cria uma tarefa e se associa com o usuário autenticado. <br>
PUT /tasks/{id}: Altera a a tarefa do id informado se ela pertencer ao usuário. <br>
DELETE /tasks/{id}: Deleta a tarefa do id informado se ela pertencer ao usuário. <br>

- Usuários: <br>
GET /users: Retorna os usuários cadastros no sistema. <br>
POST /users: Cria um usuário. <br>

- Login: <br>
POST /token: Realiza o login, se for autenticado ele retorna o token.

### Como usar
Em exceção a rota de Login, todas as outras exigem autenticação. A aplicação já disponibiliza um usuário "admin" com senha "admin" na criação do banco com o EF Core, a partir desse usuário você realiza o login, resgata o token e cria novos usuários e tarefas.

### Exemplos de Body Request
- Criar um usuário
``` json
{
    "Name": "Mateus",
    "Email": "mateus@gmail.com",
    "Password": "123"
}
```
- Criar e editar uma tarefa
``` json
{
    "Title": "Desenvolver API",
    "Description": "Desenvolver uma API .NET com JWT",
    "FinishDt": "0001-01-01"
}
```
Obs: Caso a tarefa ainda não tenha sido terminada, o valor da chave "FinishDt" será nulo, portanto deve se usar o valor "0001-01-01"(YYYY-MM-DD) que é o equivalente a nulo para o DateTime.
