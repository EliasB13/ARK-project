import { authHeader } from "../helpers";
import { responseHandler } from "../helpers";

export const readersService = {
  getReaders,
  addReader,
  removeReader
};

function getReaders() {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/Readers/all-readers`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function addReader(item) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(item)
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/Readers/add-reader`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function removeReader(id) {
  const requestOptions = {
    method: "DELETE",
    headers: { ...authHeader(), "Content-Type": "application/json" }
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/Readers/reader/${id}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}
