import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function redirectsToHttpsValidator(): ValidatorFn{
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;

        var redirectsToHttpsControl = control.get('redirectsToHttps');
        var usesHttpControl = control.get('usesHttp');
        var rawSettingsControl = control.get('rawSettings');

        console.log(redirectsToHttpsControl?.value)

        if (!rawSettingsControl?.value && redirectsToHttpsControl?.value == true && usesHttpControl?.value == true) {
            // Invalid redirect
            console.log("invalid redirect")
            return { invalidRedirect: true };
        }

        return null;
    };
}

export function usesHttpOrHttpsValidator(): ValidatorFn{
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;

        var certificateIdControl = control.get('certificateId');
        var usesHttpControl = control.get('usesHttp');
        var rawSettingsControl = control.get('rawSettings');

        if (!rawSettingsControl?.value && usesHttpControl?.value == false && certificateIdControl?.value == -1) {
            // Invalid redirect
            console.log("invalid usesHttp")
            return { usesHttpOrHttpsError: true };
        }

        return null;
    };
}

export function httpsCertificateValidator(): ValidatorFn{
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;

        var certificateIdControl = control.get('certificateId');
        var redirectsToHttpsControl = control.get('redirectsToHttps');
        var rawSettingsControl = control.get('rawSettings');

        if (!rawSettingsControl?.value && redirectsToHttpsControl?.value == true && certificateIdControl?.value == -1) {
            // Invalid redirect
            console.log("invalid certificate")
            return { invalidCert: true };
        }

        return null;
    };
}

export function rawSettingsStringValidator(minLength: number, maxLength: number, controlname: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.root.get(controlname)?.value;
        const rawSettingsControl = control.root.get('rawSettings');

        if (!value && !rawSettingsControl?.value) {
            // Invalid raw settings
            return { required: true };
        }

        if (value && value.length < minLength
             && !rawSettingsControl?.value) {
            // Invalid raw settings
            return { minLength: true };
        }

                if (value && value.length > maxLength
             && !rawSettingsControl?.value) {
            // Invalid raw settings
            return { maxLength: true };
        }

        return null;
    };
}

export function rawSettingsNumberValidator(min: number, max: number, controlname: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.root.get(controlname)?.value;
        const rawSettingsControl = control.root.get('rawSettings');

        if (!value && !rawSettingsControl?.value) {
            // Invalid raw settings
            return { required: true };
        }

        if (value && value < min
             && !rawSettingsControl?.value) {
            // Invalid raw settings
            return { min: true };
        }

                if (value && value > max
             && !rawSettingsControl?.value) {
            // Invalid raw settings
            return { max: true };
        }

        return null;
    };
}