// We can have all the properties in here instead of having all these properties as parameters in the getMembers() method in the membersService
// and make it an object instead like we will do here

import { User } from "./user";

// And why a class, why not just an interface?
// Well, a class gives us an opportunity to use a constructor, which means we can initialize some values inside the class when we use it.
export class UserParams {
    gender: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 3;
    orderBy = 'lastActive'

    constructor(user: User) {
        this.gender = user.gender == 'female' ? 'male' : 'female';
    }
}