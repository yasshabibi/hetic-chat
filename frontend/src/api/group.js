
export const createGroup = async ({ name, createdBy, memberUserIds }) => {
    const response = await fetch('https://localhost:7266/group/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, createdBy, memberUserIds }),
    });

    if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(errorBody || 'Error occurred during group creation');
    }

    return await response.json();
};
