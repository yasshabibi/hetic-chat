


export function getInitials(stringToConvert) {
    const initials = stringToConvert.match(/\b\w/g) || [];
    return ((initials.shift() || '') + (initials.pop() || '')).toUpperCase();
}
