import { authHeader } from "../helpers";
import { responseHandler } from "../helpers";

export const statisticService = {
  getFullStats,
  getReaderStats,
  getFullCountStats,
  getReaderCountStats,
  getPersonStats
};

function getFullStats(lowerBound, upperBound) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/full-statistic?lowerBound=${lowerBound}&upperBound=${upperBound}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function getReaderStats(readerId, lowerBound, upperBound) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/reader-statistic/${readerId}?lowerBound=${lowerBound}&upperBound=${upperBound}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function getFullCountStats(lowerBound, upperBound) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/full-count-statistic?lowerBound=${lowerBound}&upperBound=${upperBound}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function getReaderCountStats(readerId, lowerBound, upperBound) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/reader-count-statistic/${readerId}?lowerBound=${lowerBound}&upperBound=${upperBound}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}

function getPersonStats(cardId, lowerBound, upperBound) {
  const requestOptions = {
    method: "GET",
    headers: authHeader()
  };

  return fetch(
    `${process.env.VUE_APP_DEV_BACKEND_URL}/api/BusinessUsers/person-statistic/${cardId}?lowerBound=${lowerBound}&upperBound=${upperBound}`,
    requestOptions
  ).then(responseHandler.handleResponse);
}
