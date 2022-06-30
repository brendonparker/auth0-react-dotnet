This is a demo using Auth0 with a react front end and dotnet backend.

It also demonstrates plumbing for organizations features, which allow different pools of users.

In order to get it working, we need to setup the following rule in Auth0 to add the email and org claims to the access tokens:
```
function addEmailAndOrgToAccessToken(user, context, callback) {
  context.accessToken["https://localhost/email"] = user.email;
  context.accessToken["https://localhost/org"] = context.organization.name;
  return callback(null, user, context);
}
```