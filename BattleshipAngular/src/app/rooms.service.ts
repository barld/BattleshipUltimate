import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {
  connection: HubConnection;

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7200/Rooms")
      .build();



    this.connection.start();
  }

  public getRooms(): Observable<string[]> {
    return new Observable(sub => {
      const f: (rooms: string[]) => void = rooms => sub.next(rooms)
      this.connection.on('GetAllRooms', f);

      return () => {
        this.connection.off('GetAllRooms', f)
      }
    });
  }
}
