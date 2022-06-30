import { useEffect, useState } from "react";

const localStorageGetJson = (key) => {
  const val = localStorage.getItem(key);
  return val && JSON.parse(val);
};

export default function useLocal(key) {
  const [value, setValue] = useState(() => localStorageGetJson(key));
  useEffect(() => {
    const val = localStorageGetJson(key);
    val && setValue(val);
  }, [key]);

  return [
    value,
    (val) => {
      setValue(val);
      localStorage.setItem(key, JSON.stringify(val));
    },
  ];
}
