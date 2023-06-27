export interface Pagination {
    // We gonna have the same properties here that we are returning in our headers which is the value of the Pagination in Postman and similiar to the PaginationHeader in the Backend
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

// We gonna make this class generic, because its not just the memberDto we want to page, theres going to be other things we want to page as well just like in the backend
export class PaginatedResult<T> {
    result?: T; // The "T" is gonna be our list of things that we want to page
    pagination?: Pagination;
    // NB ON HOW THIS IS WORKING:
    // So in order to use this when we getting our response back from the api we going to need to take a look at the header and fish out the Pagination information for the pagination property
    // and create a new PaginatedResult class and populate the pagination property in this class with the Pagination information and then set the result to the list of items
    // which is in the result : T property
}