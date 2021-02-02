import { Injectable } from '@angular/core';
import { IEnvironment } from '../ienvironment';
import { environment } from '../environment';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService implements IEnvironment {

  constructor() { }

  get production(): boolean {
    return environment.production;
  }

  get apiBaseUrl(): string {
    return environment.apiBaseUrl;
  }
}
