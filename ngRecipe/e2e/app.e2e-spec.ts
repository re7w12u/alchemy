import { WebApplication1Page } from './app.po';

describe('web-application1 App', function() {
  let page: WebApplication1Page;

  beforeEach(() => {
    page = new WebApplication1Page();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
