export const registerUser = async (userData) => {
    const response = await fetch('https://localhost:7266/user/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
    });

    if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(errorBody || 'Error occurred during registration');
    }

    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
        return await response.json();
    } else {
        console.log('Received non-JSON response');
        return await response.text();
    }
};

export const loginUser = async (username, password) => {
    const response = await fetch('https://localhost:7266/User/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password }),
    });

    if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(errorBody || 'Error occurred during login');
    }

    return await response.json();
};


export const getUserData = async (userId) => {
    const response = await fetch('https://localhost:7266/user/' + userId, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userId }),
    });

    if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(errorBody || 'Error occurred while fetching user data');
    }

    return await response.json();
};