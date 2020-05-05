import { authHeader } from "../helpers";
import { responseHandler } from "../helpers";

export const cardsService = {
  getCards,
  getCardStatistic,
  addCard,
  deleteCard
};

function getCards() {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/person-cards`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function getCardStatistic(id) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/person-statistic/${roleId}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function addCard(item) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(item)
  };

  const requestString = `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/add-person-card`;

  return fetch(requestString, requestOptions).then(
    responseHandler.handleResponse
  );
}

function deleteCard(id) {
  const requestOptions = {
    method: "DELETE",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/person-card/${id}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}
