import { useState } from "react";
import { useApiV2 } from "../auth0/useApi";
import { useNavigate } from "react-router-dom";

const NewUserComponent = () => {
  const [user, setUser] = useState({ email: "" });
  const { fetchNoRes } = useApiV2();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  async function onSubmit(e) {
    e.preventDefault();
    setLoading(true);
    try {
      await fetchNoRes("/users", {
        method: "POST",
        body: JSON.stringify(user),
        headers: {
          "Content-Type": "application/json",
        },
      });
      navigate("/users");
    } catch (e) {
      console.log(e);
      setLoading(false);
    }
  }

  return (
    <div>
      <h1>New User</h1>
      <form
        className={"ui form " + (loading ? "loading" : "")}
        onSubmit={onSubmit}
      >
        <div className="field">
          <label>Email</label>
          <input
            type="email"
            value={user.email}
            onChange={(e) => setUser({ ...user, email: e.target.value })}
          />
        </div>

        <button type="submit" className="ui button primary">
          Create User
        </button>
      </form>
    </div>
  );
};

export default NewUserComponent;
