# NetCoreAPI_Practice
NetCore(3.0) Web API with local DB
Basic function with [GET]()/[POST]()/[DELETE]()
## API Route: 
### GET
/user/users --- list of all user</br>
/user/users/{name} --- get user info by name
### POST 
/user/add --- add new user with no duplicate name
```json
{
    "name":"name",
    "description":"description"
}
```
/login --- login to api get authorized token
```json
{
    "loginname":"loginName",
    "loginpwd":"pwd",
}
```
/login/add --- add new role to login
```json
{
    "name":"Name",
    "pwd":"pwd",
    "role":"role"
}
```
### DELETE
/user/{id} --- delete user by id


:zzz:
