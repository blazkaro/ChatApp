import { Component, OnInit } from '@angular/core';
import { RealTimeCommunicationService } from './core/services/real-time-communication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(private readonly rtcService: RealTimeCommunicationService) { }

  ngOnInit(): void {
    this.rtcService.openConnection();
  }
}
