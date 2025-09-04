export interface AppUser {
    email: string;
    name: string;
    surname: string;
    password: string;
    confirmPassword: string;
    nationalCode: string;
    dateOfBirth: string | undefined;
    city: string;
}