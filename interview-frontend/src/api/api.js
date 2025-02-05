const api_url = "http://localhost:10010";

export const startPlan = async () => {
    const url = `${api_url}/Plan/`;
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({}),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return await response.json();
};

export const addProcedureToPlan = async (planId, procedureId) => {
    const url = `${api_url}/Plan/AddProcedureToPlan`;
    var command = { planId: planId, procedureId: procedureId };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return true;
};

export const assignUserToProcedure = async (procedureId,userIds) => {
    const url = `${api_url}/Procedures/AddUsertoProcedure`;
    var command = { procedureId: procedureId,userIds: userIds };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });

    if (!response.ok) throw new Error("Failed to assign user to procedure");

    return true;
};

export const getProcedures = async () => {
    const url = `${api_url}/Procedures`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get procedures");

    return await response.json();
};

export const getPlanProcedures = async (planId) => {
    const url = `${api_url}/PlanProcedure?$filter=planId eq ${planId}&$expand=procedure`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get plan procedures");

    return await response.json();
};


export const getProcedureUsers = async (procedureId) => {
    try {
        const response = await fetch(`${api_url}/ProcedureUser/GetUsersByProcedure/${procedureId}`);
        if (!response.ok) {
            throw new Error("Error fetching users");
        }
        const data = await response.json(); // Assuming the response is JSON
        return data; // Returns the user data
    } catch (error) {
        console.error("Error fetching data:", error);
        throw error; // Optional: rethrow error if you want to handle it in the component
    }
};


export const getUsers = async () => {
    const url = `${api_url}/Users`;
    const response = await fetch(url, {
        method: "GET",
    });
    console.log("Users:", response); // ✅ Debug
    if (!response.ok) throw new Error("Failed to get users");

    return await response.json();
};

