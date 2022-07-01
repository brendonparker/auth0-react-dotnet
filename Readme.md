This is a demo using Auth0 with a react front end and dotnet backend.

It also demonstrates plumbing for organizations features, which allow different pools of users.

In order to get it working, we need to setup the following rule in Auth0 to add the email and org claims to the access tokens:
```
function addEmailAndOrgToAccessToken(user, context, callback) {
  context.accessToken["https://localhost/email"] = user.email;
  context.accessToken["https://localhost/org"] = context.organization.name;
  context.accessToken["https://localhost/org_id"] = context.organization.id;
  return callback(null, user, context);
}
```

For the local api to work, you need to setup the following secrets:
```
dotnet user-secrets set "Auth0:ClientId" "YOUR_CLIENT_ID"
dotnet user-secrets set "Auth0:ClientSecret" "YOUR_CLIENT_SECRET"
dotnet user-secrets set "Auth0:Domain" "YOUR_DOMAIN"
```