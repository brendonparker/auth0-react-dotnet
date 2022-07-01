import { useApi } from "../auth0/useApi";
const UsersComponent = () => {
  const { data } = useApi("/users");
  return (
    <>
      <h1>Users</h1>
      <table>
        <thead>
          <tr>
            <th>Email</th>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {data?.map((user) => (
            <tr key={user.userId}>
              <td>{user.email}</td>
              <td>{user.name}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default UsersComponent;
