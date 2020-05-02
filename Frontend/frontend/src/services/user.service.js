import { authHeader } from "../helpers";
import { responseHandler } from "../helpers";

export const userService = {
  login,
  logout,
  register,
  update,
  delete: _delete,
  getAccountData
};

function login(login, password) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ login, password })
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/authenticate-business`;

  return fetch(requestString, requestOptions)
    .then(responseHandler.handleResponse)
    .then(user => {
      if (user.token) localStorage.setItem("user", JSON.stringify(user));

      return user;
    });
}

function logout() {
  localStorage.removeItem("user");
}

function register(user) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(user)
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/register-business`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function getAccountData() {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/account-data`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function update(user) {
  const requestOptions = {
    method: "PUT",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(user)
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function _delete(id) {
  const requestOptions = {
    method: "DELETE",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/${id}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}
