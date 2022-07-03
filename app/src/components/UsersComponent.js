import { NavLink } from "react-router-dom";
import { useApi, useApiV2 } from "../auth0/useApi";
const UsersComponent = () => {
  const { data, refresh } = useApi("/users");
  const { fetchNoRes } = useApiV2();
  async function onDelete(userId) {
    await fetch("/users/" + userId, {
      method: "DELETE",
    });
    refresh();
  }
  async function onChangeRole(user, role) {
    await fetchNoRes(`/users/${user.userId}/role`, {
      method: "PUT",
      body: JSON.stringify({ role }),
      headers: {
        "Content-Type": "application/json",
      },
    });
    refresh();
  }
  return (
    <>
      <h1>Users</h1>
      <NavLink to="/user/create">Create New User</NavLink>
      <table>
        <thead>
          <tr>
            <th>Email</th>
            <th>Name</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {data?.map((user) => (
            <tr key={user.userId}>
              <td>{user.email}</td>
              <td>{user.name}</td>
              <td>
                <select
                  value={user.roles[0] || "User"}
                  onChange={(e) => onChangeRole(user, e.target.value)}
                >
                  <option value="User">User</option>
                  <option value="Power User">Power User</option>
                  <option value="Admin">Admin</option>
                </select>
              </td>
              <td>
                <button onClick={() => onDelete(user.userId)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default UsersComponent;
