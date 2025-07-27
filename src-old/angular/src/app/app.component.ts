
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TestServiceProxy } from '../shared/service-proxies';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // SQL
  insertName = '';
  insertDescription = '';

  readId: number = 0;

  // Queue
  queueMessage = '';

  // Redis
  cacheKey = '';
  cacheValue = '';

  constructor(private tsp: TestServiceProxy) {

  }

  insertData() {
    this.tsp.insertData(this.insertName, this.insertDescription).subscribe(
      result => {
        console.log('Insert Result:', result);
      },
      error => {
        console.error('Insert Error:', error);
      }
    );
  }

  readData() {
    
    this.tsp.readData(this.readId).subscribe(
      result => {
        window.alert(`Read Result: ${JSON.stringify(result)}`);
      },
      error => {
        console.error('Read Error:', error);
      }
    );
  }

  sendMessageToQueue() {
    this.tsp.sendMessageToQueue(this.queueMessage).subscribe(
      result => {
        console.log('Insert Result:', result);
      },
      error => {
        console.error('Queue Send Error:', error);
      }
    );
  }

  readMessageFromQueue() {
    this.tsp.readMessageFromQueue().subscribe(
      result => {
        window.alert(`Queue Read Result: ${result}`);
        // window.alert(`Queue Read Result: ${JSON.stringify(result)}`);
      },
      error => {
        console.error('Queue Read Error:', error);
      }
    );
  }

  setCache() {
    this.tsp.setCache(this.cacheKey, this.cacheValue).subscribe(
      result => {
        console.log('Cache Set Result:', result);
      },
      error => {
        console.error('Cache Set Error:', error);
      }
    );
  }

  getCache() {
    this.tsp.getCache(this.cacheKey).subscribe(
      result => {
        window.alert(`Cache Get Result: ${result}`);
        // window.alert(`Cache Get Result: ${JSON.stringify(result)}`);
      },
      error => {
        console.error('Cache Get Error:', error);
      }
    );
  }
}
