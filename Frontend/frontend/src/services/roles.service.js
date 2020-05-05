import { authHeader } from "../helpers";
import { responseHandler } from "../helpers";

export const rolesService = {
  getRoles
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
