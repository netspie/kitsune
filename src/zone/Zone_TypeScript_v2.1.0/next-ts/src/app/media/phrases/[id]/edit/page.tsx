import MainLayout from 'src/layouts/main';
import PhraseViewEdit from 'src/sections/_media/view/phrase-view-edit';

// ----------------------------------------------------------------------

export const metadata = {
  title: 'Phrase - やっと試験終わったね - Edit',
};

export default function PhraseEdit() {
  return (
    <MainLayout>
      <PhraseViewEdit />
    </MainLayout>
  );
}
