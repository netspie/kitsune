import MainLayout from 'src/layouts/main';
import PhraseView from 'src/sections/_media/view/phrase-view';

// ----------------------------------------------------------------------

export const metadata = {
  title: 'Phrase - やっと試験終わったね',
};

export default function Phrase() {
  return (
    <MainLayout>
      <PhraseView />
    </MainLayout>
  );
}
