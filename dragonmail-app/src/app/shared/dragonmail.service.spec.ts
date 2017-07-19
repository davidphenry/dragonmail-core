import { TestBed, inject } from '@angular/core/testing';

import { DragonmailService } from './dragonmail.service';

describe('DragonmailService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DragonmailService]
    });
  });

  it('should be created', inject([DragonmailService], (service: DragonmailService) => {
    expect(service).toBeTruthy();
  }));
});
