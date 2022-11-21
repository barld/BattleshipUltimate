import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { RoomsService } from './rooms.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'BattleshipAngular';
  rooms: Observable<string[]>;

  constructor(private readonly roomsService: RoomsService) {

    this.rooms = roomsService.getRooms()
  }
}
