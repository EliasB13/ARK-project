import { authHeader } from "../helpers";
import { responseHandler } from "../helpers";

export const rolesService = {
  getRoles,
  getRoleById,
  getRoleEmployees,
  getRoleRestrictedReaders,
  addRole,
  addCardToRole,
  deleteRole,
  restrictReader,
  unrestrictReader
};

function getRoles() {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/all-roles`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function getRoleById(id) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/role/${id}`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function getRoleEmployees(id) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/employees-in-roles/${id}`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function getRoleRestrictedReaders(id) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/restricted-role-readers/${id}`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function addRole(item) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(item)
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/add-role`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function addCardToRole(roleId, cardId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ roleId: roleId, cardId: cardId })
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/add-employee-to-role`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function deleteRole(id) {
  const requestOptions = {
    method: "DELETE",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/role/${id}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function restrictReader(readerId, roleId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ readerId: readerId, roleId: roleId })
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/restrict-reader`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function unrestrictReader(readerId, roleId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ readerId: readerId, roleId: roleId })
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/EmployeesRoles/unrestrict-reader`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}
