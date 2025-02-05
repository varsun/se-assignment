import React, { useState, useEffect } from "react";
import ReactSelect from "react-select";
import {
    assignUserToProcedure,
    getProcedureUsers,
} from "../../../api/api";

const PlanProcedureItem = ({ procedure, users }) => {
        const [selectedUsers, setSelectedUsers] = useState([]);

    useEffect(() => {
        (async () => {
        try
        {
            var Sel_users = await getProcedureUsers(procedure.procedureId);
            if (Array.isArray(Sel_users)) 
                {
                const userselected = Sel_users.map(u => ({
                    value: u.userId,  // Use userId as the value
                    label: u.name     // Use name as the label
                }));  
            setSelectedUsers(userselected); // ✅ Corrected state update
            }
            else
            {
                console.error("Fetched users is not an array:", Sel_users);
            }

        }
        catch (error) 
        {
            console.error("Error fetching assigned users:", error);
        }
          })();
      }, [procedure.procedureId]);

    const handleAssignUserToProcedure = async (selectedOptions) => {
        setSelectedUsers(selectedOptions); // ✅ Update state with selected users
        
        try 
        {
           const userIds = selectedOptions.map(user => user.value); // Extract user IDs
           const response = await assignUserToProcedure(procedure.procedureId, userIds);
            if (response) 
            {
               console.log("Users assigned successfully:", response);
               alert("Users successfully assigned! ✅");
            }
        } 
        catch (error) 
        {
            console.error("Failed to assign users:", error);
            alert("Failed to assign users : " + error);
        }  
    };
    
    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange = {handleAssignUserToProcedure}
            />
           
        </div>
    );
};

export default PlanProcedureItem;
