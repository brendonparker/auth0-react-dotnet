import { useState } from "react";
import { useApiV2 } from "../auth0/useApi";

const NewUserComponent = () => {
  const [user, setUser] = useState({ email: "" });
  const { fetch } = useApiV2();

  async function onSubmit(e) {
    e.preventDefault();
    await fetch("/users", {
      method: "POST",
      body: JSON.stringify(user),
      headers: {
        "Content-Type": "application/json",
      },
    });
  }

  return (
    <div>
      <h1>New User</h1>
      <form onSubmit={onSubmit}>
        <input
          type="email"
          value={user.email}
          onChange={(e) => setUser({ ...user, email: e.target.value })}
        />
        <button type="submit">Create User</button>
      </form>
    </div>
  );
};

export default NewUserComponent;
