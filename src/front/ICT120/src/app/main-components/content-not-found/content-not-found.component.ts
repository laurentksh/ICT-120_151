import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-content-not-found',
  templateUrl: './content-not-found.component.html',
  styleUrls: ['./content-not-found.component.css']
})
export class ContentNotFoundComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  goHome(): void {
    this.router.navigate(['/home'])
  }

}
