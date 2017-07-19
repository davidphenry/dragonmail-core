import { DragonmailAppPage } from './app.po';

describe('dragonmail-app App', () => {
  let page: DragonmailAppPage;

  beforeEach(() => {
    page = new DragonmailAppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
