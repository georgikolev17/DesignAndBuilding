# Project Overview: Platform for Forming Design Teams in Construction via Real-Time Sealed-Bid Auctions

## Summary
[Visit Website](https://buildnet.bg)
This platform is designed to bring together all participants in the architectural and engineering design process in Bulgaria into a single network. It is intended for:

- All architects in Bulgaria who are members of the **Chamber of Architects in Bulgaria (CAB)**
- All engineers with full design licenses who are members of the **Chamber of Engineers in Investment Design (CEID)**

The main goal of the platform is to help architects form the optimal team of engineers for each specific project they are assigned.

---

## How the System Works

Once an architect is assigned a project, they can enter it into the platform using the following process:

1. **Enter Basic Project Information** (required)  
   Information such as project name, location, and brief description.

2. **Create Assignments for Engineering Disciplines** (required)  
   Assignments are created for each relevant part of the investment project, such as:
   - Structural Engineering
   - Electrical Systems
   - Water Supply and Sewerage
   - HVAC (Heating, Ventilation, and Air Conditioning)
   - And others

   This is done through a specialized form. Additional notes can be added as free text (optional).

3. **Invitation to Bid**  
   After a project is registered, all engineers specializing in the relevant disciplines are notified and gain access to the assignments for their specialty.

4. **Real-Time Sealed-Bid Auction**  
   Engineers can submit their bids for the project. The auction is:
   - Sealed-bid: Participants do not see each other’s exact bids.
   - Real-time: Movement in bid positions can be monitored as bids are placed.
   - Time-limited: The architect sets a deadline (date and time) for bidding.

5. **Results and Selection**  
   At the end of the auction:
   - The architect receives a notification with the final rankings.
   - Full contact details of all participating engineers are revealed.
   - The architect can choose any participant to collaborate with (not necessarily the top-ranked one).

---

## Technologies Used

- **ASP.NET MVC** – Web application framework
- **SQL Database** – Data storage and management
- **SignalR** – WebSocket-based real-time communication for the live bidding system

---

## Future Enhancements

- Rating and review system for both architects and engineers
- Project portfolio integration for each participant
- Notification settings and customization
- Multilingual support

---
